import './App.css';
import { useEffect, useRef, useState } from 'react';
import { Unity, useUnityContext } from 'react-unity-webgl';
import ButtonGroup from '@mui/material/ButtonGroup';
import Select from 'react-select';
import Button from '@mui/material/Button';
import Grid from '@mui/material/Grid';
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
  const holisticRef = useRef<Holistic | null>(null);
  const cameraRef = useRef<HTMLVideoElement>(null);
  const [devices, setDevices] = useState<MediaDeviceInfo[]>([]);
  const [counter, setCounter] = useState(0);
  const [currentCam, setCurrentCam] = useState({ value: "", label: "" });

  const { unityProvider, isLoaded, sendMessage, takeScreenshot } = useUnityContext({
    loaderUrl: "WebGL/Build/WebGL.loader.js",
    dataUrl: "WebGL/Build/WebGL.data.unityweb",
    frameworkUrl: "WebGL/Build/WebGL.framework.js.unityweb",
    codeUrl: "WebGL/Build/WebGL.wasm.unityweb",
    webglContextAttributes: { preserveDrawingBuffer: true }
  });

  const fetchTemplates = () => {
    setTemplates([
      {
        id: 1,
        description: "BAYC",
        assetUri: "https://10k-asset.s3.ap-southeast-1.amazonaws.com/mock/collection_mas_metadata.zip"
      }
    ])
  }

  const fetchTokens = () => {
    setTokens(["0", "1", "2", "3", "4", "5"].map((id: string) =>
      ({ tokenId: id, assetUri: `https://10k-asset.s3.ap-southeast-1.amazonaws.com/mock/${id}.zip` })
    ))
  }

  const avatarOptions = tokens.map((token, idx) => ({ value: idx, label: templates[0].description + token.tokenId }))

  useEffect(() => {
    fetchTemplates()
    fetchTokens()
  }, [])

  function loadAvatar(assetId: number, templateUri: string, tokenId: string, tokenUri: string) {
    sendMessage("MessageHandler", "LoadAvatar", JSON.stringify({
      assetVersionId: assetId,
      templateAssetUri: templateUri,
      tokenId: tokenId,
      tokenAssetUri: tokenUri
    }));
  }

  function setARMode(str: string) {
    sendMessage("MessageHandler", "SetARMode", str);
  }

  function selectAvatar(assetId: number, tokenId: string) {
    sendMessage("MessageHandler", "SelectAvatar", JSON.stringify({
      assetVersionId: assetId,
      tokenId: tokenId
    }))
  }

  useEffect(() => {
    const device$ = from(navigator.mediaDevices.enumerateDevices());
    const subs = device$.subscribe((args) => {
      const videoInputDevices = args.filter(device => device.kind == "videoinput");
      setDevices(args)
    })

    return () => {
      subs.unsubscribe();
    }
  }, [setDevices, setCurrentCam]);

  const deviceOptions = devices.filter(device => device.kind == 'videoinput').map((device: MediaDeviceInfo) => ({ value: device.deviceId, label: device.label }));

  function onCameraChanged(value: string, label: string) {
    setCurrentCam({ value: value, label: label });
  }

  useEffect(() => {
    if (devices.length == 0) return;
    if (currentCam.value == '') return;

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
      if (lastSource && lastSource.srcObject) {
        const stream = lastSource!.srcObject as MediaStream;
        const tracks = stream!.getTracks();
        tracks.forEach((track) => {
          track.stop();
        })
      }
    }
  }, [devices, currentCam])

  useEffect(() => {
    if (devices.length == 0) return;
    if (currentCam.value == '') return;
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
      smoothLandmarks: true
    })

    holisticRef.current.onResults((result) => {
      const motionData = {
        face: result.faceLandmarks,
        pose: result.poseLandmarks,
        width: cameraRef.current?.videoWidth,
        height: cameraRef.current?.videoHeight
      }
      setCounter((x) => x + 1);
      sendMessage("MessageHandler", "ProcessCapturedResult", JSON.stringify(motionData));
    })
  }, [sendMessage, setCounter, currentCam])

  async function updateFunc() {
    if (cameraRef.current && holisticRef.current) {
      await holisticRef.current.send({ image: cameraRef.current });
    }
  }

  function captureUnityScreen() {
    takeScreenshotWithWebCam().then((captured) => {
      const img = document.createElement('img');
      img.src = captured;
      img.style.maxWidth = '100%';

      const container = document.getElementById('image-container');
      if (container) {
        container.innerHTML = '';
        container.appendChild(img);
      } else {
        console.error('Image container not found');
      }
    });
  }

  function takeScreenshotWithWebCam(): Promise<string> {
    return new Promise((resolve, reject) => {
      const canvas = document.createElement('canvas');
      const ctx = canvas.getContext('2d');

      if (!ctx) {
        reject(new Error('Canvas context is null'));
        return;
      }

      if (!cameraRef.current) {
        reject(new Error('No Camera Ref'));
        return;
      }

      canvas.width = cameraRef.current.videoWidth;
      canvas.height = cameraRef.current.videoHeight;

      ctx.translate(canvas.width, 0);
      ctx.scale(-1, 1);
      ctx.drawImage(cameraRef.current!, 0, 0, canvas.width, canvas.height);

      var unityCaptured = takeScreenshot("image/png", 1.0);

      if (!unityCaptured) {
        reject(new Error('Failed to capture Unity screen'));
        return;
      }

      const image = new Image();
      image.src = unityCaptured;
      image.onload = () => {
        const tempCanvas = document.createElement('canvas');
        const tempCtx = tempCanvas.getContext('2d');
        tempCanvas.width = canvas.width;
        tempCanvas.height = canvas.height;

        if (!tempCtx) {
          reject(new Error('Temporary canvas context is null'));
          return;
        }

        tempCtx.translate(tempCanvas.width, 0);
        tempCtx.scale(-1, 1);
        tempCtx.drawImage(image, 0, 0, tempCanvas.width, tempCanvas.height);

        ctx.drawImage(tempCanvas, 0, 0, canvas.width, canvas.height);

        const captured = canvas.toDataURL('image/png');

        canvas.remove();
        tempCanvas.remove();

        resolve(captured);
      };
    });
  }

  useEffect(() => {
    if (counter > 0)
      updateFunc();
  }, [counter])

  type VideoData = {
    chunks: Blob[];
  };

  const mediaRecorder = useRef<MediaRecorder | null>(null);
  const frameRate = 30;

  const startRecording = async () => {
    const canvas = document.createElement('canvas');
    const context = canvas.getContext('2d');
    canvas.width = 720;
    canvas.height = 480;

    const stream = canvas.captureStream(frameRate);

    const audioStream = await navigator.mediaDevices.getUserMedia({ audio: true });

    const combinedStream = new MediaStream([...stream.getVideoTracks(), ...audioStream.getAudioTracks()]);

    mediaRecorder.current = new MediaRecorder(combinedStream, { mimeType: 'video/webm' });
    mediaRecorder.current.ondataavailable = (e) => {
      saveVideo({ chunks: [e.data] });
    };

    mediaRecorder.current.start();

    const drawFrame = () => {
      if (!mediaRecorder.current || mediaRecorder.current.state !== 'recording') {
        return;
      }

      if (context) {
        takeScreenshotWithWebCam().then((combined) => {
          if (combined) {
            const image = new Image();

            image.src = combined;

            image.onload = function () {
              context.drawImage(image, 0, 0, canvas.width, canvas.height);
              requestAnimationFrame(drawFrame);
            }
          }
        })
      }
    };

    drawFrame();
  };

  const stopRecording = () => {
    if (mediaRecorder.current) {
      mediaRecorder.current.stop();
    }
  };

  const saveVideo = (video: VideoData) => {
    if (video.chunks.length === 0) {
      console.error('No video data to save');
      return;
    }

    const blob = new Blob(video.chunks, { type: 'video/webm' });
    const url = URL.createObjectURL(blob);
    const a = document.createElement('a');
    a.href = url;
    a.download = 'recording.webm';
    a.click();
    URL.revokeObjectURL(url);
  };

  useEffect(() => {
    if (devices.length == 0) return;
    if (currentCam.value == '') return;
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
      smoothLandmarks: true
    })

    holisticRef.current.onResults((result) => {
      const motionData = {
        face: result.faceLandmarks,
        pose: result.poseLandmarks,
        width: cameraRef.current?.videoWidth,
        height: cameraRef.current?.videoHeight
      }
      setCounter((x) => x + 1);
      sendMessage("MessageHandler", "ProcessCapturedResult", JSON.stringify(motionData));
    })
  }, [sendMessage, setCounter, currentCam])

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
          <Select options={avatarOptions} onChange={(item) => { if (item != null) { setSelectedToken(item!.value) } }} />
        </Grid>
        <Grid item xs={5}>
          <ButtonGroup variant="contained" aria-label="outlined primary button group">
            <Button onClick={(item) => loadAvatar(templates[0].id, templates[0].assetUri, tokens[selectedToken].tokenId, tokens[selectedToken].assetUri)}>Load Avatar</Button>
            <Button onClick={(item) => setARMode("true")}>Set AR Mode</Button>
            <Button onClick={(item) => setARMode("false")}>Disable AR Mode</Button>
            <Button onClick={(item) => selectAvatar(templates[0].id, tokens[selectedToken].tokenId)}>Select Avatar</Button>
            <Button onClick={(item) => captureUnityScreen()}>Capture</Button>
            <Button onClick={(item) => startRecording()}>Start Recording</Button>
            <Button onClick={(item) => stopRecording()}>Stop Recording</Button>
          </ButtonGroup>
        </Grid>
        <Grid item xs={5}>
          <div style={{ position: 'relative', width: '720px', height: '480px' }}>
            <Unity
              style={{
                position: 'absolute',
                top: 0,
                left: 0,
                zIndex: 2,
                visibility: isLoaded ? 'visible' : 'hidden',
                width: '720px',
                height: '480px',
              }}
              unityProvider={unityProvider}
            />
            <video
              ref={cameraRef}
              autoPlay={true}
              onLoadedData={updateFunc}
              style={{
                position: 'absolute',
                top: 0,
                left: 0,
                zIndex: 1,
                transform: "scaleX(-1)",
                width: '720px',
                height: '480px',
              }}
            />
          </div>
        </Grid>
        <Grid item xs={10}>
          <div id="image-container"></div>
        </Grid>
      </Grid>
    </div>
  );
}

export default App;
