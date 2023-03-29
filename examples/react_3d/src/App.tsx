import './App.css';
import { Button, ButtonGroup, Container, Grid } from '@mui/material';
import { useEffect, useRef, useState } from 'react';
import { Unity, useUnityContext } from 'react-unity-webgl';
import { Holistic } from '@mediapipe/holistic';
import { from } from 'rxjs';
import Select from 'react-select';

function App() {
  const holisticRef = useRef<Holistic|null>(null);
  const cameraRef = useRef<HTMLVideoElement>(null);
  const [devices, setDevices] = useState<MediaDeviceInfo[]>([]);
  const [counter, setCounter] = useState(0);
  const [currentCam, setCurrentCam] = useState({ value: "", label: ""});

  const { unityProvider, isLoaded, sendMessage } = useUnityContext({
    loaderUrl: "WebGL/Build/WebGL.loader.js",
    dataUrl: "WebGL/Build/WebGL.data",
    frameworkUrl: "WebGL/Build/WebGL.framework.js",
    codeUrl: "WebGL/Build/WebGL.wasm",
  })

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

  useEffect(() => {
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
      sendMessage("MessageHandler", "ProcessMediapipe", JSON.stringify(motionData));
    })
  }, [sendMessage, setCounter, currentCam])

  async function updateFunc() {
    if(cameraRef.current && holisticRef.current) {
      await holisticRef.current.send({image: cameraRef.current});
    }
  }

  useEffect(() => {
    if (counter > 0)
      updateFunc();
  }, [counter])

  function Load3DAvatar() {
    sendMessage("MessageHandler", "Load3DAvatar", '');
  }
  
  function Load3DTraits() {
    sendMessage("MessageHandler", "Load3DTraits");
  }

  return (
    <div className="App">
      <Grid item xs={10}>
        <p> 3D Avatar Loader </p>
      </Grid>
      <Grid item xs={10}>
        <Select options={deviceOptions} onChange={(item) => onCameraChanged(item!.value, item!.label)} />
      </Grid>
      <Grid item xs={5}>
        <ButtonGroup variant="contained" aria-label="outlined primary button group">
          <Button onClick={Load3DAvatar}>Load 3D Avatar</Button>
          <Button onClick={Load3DTraits}>Load 3D Traits</Button>
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
    </div>
  );
}

export default App;
