import './App.css';
import { useEffect, useRef, useState } from 'react';
import { Unity, useUnityContext } from 'react-unity-webgl';
import ButtonGroup from '@mui/material/ButtonGroup';
import Select from 'react-select';
import Button from '@mui/material/Button';
import Grid from '@mui/material/Grid';
import { Container } from '@mui/material';
import { Holistic } from '@mediapipe/holistic';
import { from } from 'rxjs';

export type AssetVersion = {
  id: number,
  description: string,
  assetUri: string
}

export type Asset = {
  tokenId: string,
  assetUri: string
}

function App() {
  const [templates, setTemplates] = useState<AssetVersion[]>([]);
  const [tokens, setTokens] = useState<Asset[]>([]);
  const [selectedToken, setSelectedToken] = useState<number>(0);
  const holisticRef = useRef<Holistic|null>(null);
  const cameraRef = useRef<HTMLVideoElement>(null);
  const [devices, setDevices] = useState<MediaDeviceInfo[]>([]);
  const [counter, setCounter] = useState(0);
  const [currentCam, setCurrentCam] = useState({ value: "", label: ""});

  const { unityProvider, isLoaded, sendMessage } = useUnityContext({
    loaderUrl: "WebGL/Build/WebGL.loader.js",
    dataUrl: "WebGL/Build/WebGL.data.unityweb",
    frameworkUrl: "WebGL/Build/WebGL.framework.js.unityweb",
    codeUrl: "WebGL/Build/WebGL.wasm.unityweb",
  });

  const fetchTemplates = () => {
    // fetch("https://api-dev.myty.space/asset/versions")
    //   .then(response => {
    //     return response.json()
    //   })
    //   .then(data => {
    //     setTemplates(data as AssetVersion[])
    //   })
    setTemplates([
      {
        id: 1,
        description: "BAYC",
        assetUri: "https://10k-asset.s3.ap-southeast-1.amazonaws.com/mock/collection_mas_metadata.zip"
      }
    ])
  }

  const fetchTokens = () => {
    // fetch("https://api-dev.myty.space/assets")
    //   .then(response => {
    //     return response.json()
    //   })
    //   .then(data => {
    //     setTokens(data)
    //   })
    setTokens(["0", "1", "2", "3", "4", "5"].map((id: string) => 
      ({tokenId: id, assetUri: `https://10k-asset.s3.ap-southeast-1.amazonaws.com/mock/${id}.zip`})
    ))
  }

  const avatarOptions = tokens.map((token, idx) => ({value: idx, label: templates[0].description + token.tokenId}))

  useEffect(() =>{
    fetchTemplates()
    fetchTokens()
  }, [])

  function loadAvatar(assetId: number, templateUri: string, tokenId: string, tokenUri: string)
  {
    sendMessage("MessageHandler", "LoadAvatar", JSON.stringify({
      assetVersionId: assetId,
      templateAssetUri: templateUri,
      tokenId: tokenId,
      tokenAssetUri: tokenUri
    }));
  }

  function setARMode(str: string)
  {
    sendMessage("MessageHandler", "SetARMode", str);
  }

  function selectAvatar(assetId: number, tokenId: string)
  {
    sendMessage("MessageHandler", "SelectAvatar", JSON.stringify({
      assetVersionId: assetId,
      tokenId: tokenId
    }))
  }

  useEffect(() => {
    const device$ = from(navigator.mediaDevices.enumerateDevices());
    const subs = device$.subscribe((args) => {
      setCurrentCam( { value : args[0].deviceId, label : args[0].label });
      setDevices(args)
    })

    return () => {
      subs.unsubscribe();
    }
  }, [setDevices, setCurrentCam]);

  const deviceOptions = devices.map((device: MediaDeviceInfo) => ({ value: device.deviceId, label: device.label }));

  function onCameraChanged(value: string, label: string) {
    setCurrentCam({value: value, label: label});
  }

  useEffect( () => {
    if(devices.length == 0) return;
    if(currentCam.value == '') return;

    const camera = devices.filter(dev => dev.deviceId == currentCam.value && dev.kind == 'videoinput')
    const lastSource = cameraRef.current;
    const subs$ = from(
      navigator.mediaDevices.getUserMedia({
        audio: false,
        video: { deviceId: camera[0].deviceId, width: 720, height: 480 },
      })
    ).subscribe((stream) => {
      cameraRef.current!.srcObject = stream;
      cameraRef.current!.play();
    })

    return () => {
      subs$.unsubscribe();
      const stream = lastSource!.srcObject as MediaStream;
      const tracks = stream!.getTracks();
      tracks.forEach( (track) => {
        track.stop();
      })
    }
  }, [devices, currentCam])

    useEffect(() => {
    holisticRef.current = new Holistic({
      locateFile: (file) => {
        return `https://cdn.jsdelivr.net/npm/@mediapipe/holistic/${file}`;
      }
    })

    holisticRef.current.setOptions({
      selfieMode: true,
      minDetectionConfidence: 0.5,
      minTrackingConfidence: 0.5,
      modelComplexity: 1,
      smoothLandmarks:true
    })

    holisticRef.current.onResults( (result) => {
      const motionData = {
        face: result.faceLandmarks,
        pose: result.poseLandmarks,
        width: cameraRef.current?.videoWidth,
        height: cameraRef.current?.videoHeight
      }
      setCounter((x) => x+1);
      sendMessage("MessageHandler", "ProcessCapturedResult", JSON.stringify(motionData));
    })
  }, [sendMessage,setCounter,currentCam])

  async function updateFunc() {
    if(cameraRef.current && holisticRef.current) {
      await holisticRef.current.send({image: cameraRef.current});
    }
  }

  useEffect(() => {
    if (counter > 0)
      updateFunc();
  }, [counter])

  return (
    <div className="App">
      <Grid container spacing={2}>
        <Grid item xs={10}>
          <p> 2D Avatar Loader </p>
        </Grid>
        <Grid item xs={10}>
          <Select options={deviceOptions} onChange={(item) => onCameraChanged(item!.value, item!.label)} />
        </Grid>
        <Grid item xs={10}>
          <Select options={avatarOptions} onChange={(item) => {if (item != null) {setSelectedToken(item!.value)}}}/>
        </Grid>
        <Grid item xs={5}>
          <ButtonGroup variant="contained" aria-label="outlined primary button group">
            <Button onClick={(item) => loadAvatar(templates[0].id, templates[0].assetUri, tokens[selectedToken].tokenId, tokens[selectedToken].assetUri)}>Load Avatar</Button>
            <Button onClick={(item) => setARMode("true")}>Set AR Mode</Button>
            <Button onClick={(item) => setARMode("false")}>Disable AR Mode</Button>
            <Button onClick={(item) => selectAvatar(templates[0].id, tokens[selectedToken].tokenId)}>Select Avatar</Button>
          </ButtonGroup>
        </Grid>
        <Grid item xs={5}>
          <Container>
            <Unity style={{ visibility: isLoaded ? "visible" : "hidden", width: "720px", height: "480px"}} unityProvider={unityProvider}/>
          </Container>
        </Grid>
        <Grid item xs={10}>
          <video ref={cameraRef} autoPlay={true} onLoadedData={updateFunc} hidden={true}/>
        </Grid>
      </Grid>
    </div>
  );
}

export default App;
