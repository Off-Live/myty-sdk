import '../App.css';
import { useEffect, useState, useContext, ChangeEvent } from 'react';
import ButtonGroup from '@mui/material/ButtonGroup';
import Select from 'react-select';
import Button from '@mui/material/Button';
import Grid from '@mui/material/Grid';
import { MYTYSDKContext, mytySDKContext, CalibrationType, ARFaceControlType } from "../context/MYTYSDKContext"
import VideoDeviceSelect from './VideoDeviceSelect';
import MYTYSDKView from './MYTYSDKView';
import useCaptureRecord from '../hooks/useCaptureRecord';
import { Slider, Typography } from '@mui/material';

export type AssetVersion = {
  id: number,
  description: string,
  assetUri: string
}

export type Asset = {
  tokenId: string,
  assetUri: string
}

function ExampleView() {
  const { loadAvatar, selectAvatar, switchMode, updateCalibration, controlARFace } = useContext(mytySDKContext) as MYTYSDKContext
  const { captureImage, stopRecordingVideo, startRecordingVideo } = useCaptureRecord();
  const [templates, setTemplates] = useState<AssetVersion[]>([]);
  const [tokens, setTokens] = useState<Asset[]>([]);
  const [selectedToken, setSelectedToken] = useState<number>(0);
  const [isARMode, setIsARMode] = useState<boolean>(false);

  const [syncedBlinkScale, setSyncedBlinkScale] = useState<number>(50);
  const [blinkScale, setBlinkScale] = useState<number>(100);
  const [eyebrowScale, setEyebrowScale] = useState<number>(100);
  const [pupilScale, setPupilScale] = useState<number>(100);
  const [mouthXScale, setMouthXScale] = useState<number>(100);
  const [mouthYScale, setMouthYScale] = useState<number>(100);

  const [arFaceXOffset, setARFaceXoffset] = useState<number>(0)
  const [arFaceYOffset, setARFaceYoffset] = useState<number>(0)
  const [arFaceScale, setARFaceScale] = useState<number>(1)

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

  function switchARMode() {
    setIsARMode(!isARMode)
    switchMode()
  }

  const avatarOptions = tokens.map((token, idx) => ({ value: idx, label: templates[0].description + token.tokenId }))

  useEffect(() => {
    fetchTemplates()
    fetchTokens()
  }, [])

  const handleSyncedBlinkSlider = (event: Event, newValue: number | number[]) => {
    setSyncedBlinkScale(newValue as number)
    updateCalibration(CalibrationType.SyncedBlink, newValue as number / 100)
  }

  const handleBlinkSlider = (event: Event, newValue: number | number[]) => {
    setBlinkScale(newValue as number)
    updateCalibration(CalibrationType.Blink, newValue as number / 100)
  };

  const handleEyebrowSlider = (event: Event, newValue: number | number[]) => {
    setEyebrowScale(newValue as number)
    updateCalibration(CalibrationType.Eyebrow, newValue as number / 100)
  };

  const handlePupilSlider = (event: Event, newValue: number | number[]) => {
    setPupilScale(newValue as number)
    updateCalibration(CalibrationType.Pupil, newValue as number / 100)
  };

  const handleMouthXSlider = (event: Event, newValue: number | number[]) => {
    setMouthXScale(newValue as number)
    updateCalibration(CalibrationType.MouthX, newValue as number / 100)
  };

  const handleMouthYSlider = (event: Event, newValue: number | number[]) => {
    setMouthYScale(newValue as number)
    updateCalibration(CalibrationType.MouthY, newValue as number / 100)
  };

  const handleARFaceXOffset = (event: Event, newValue: number | number[]) => {
    setARFaceXoffset(newValue as number)
    controlARFace(ARFaceControlType.XOffset, newValue as number)
  }

  const handleARFaceYOffset = (event: Event, newValue: number | number[]) => {
    setARFaceYoffset(newValue as number)
    controlARFace(ARFaceControlType.YOffset, newValue as number)
  }

  const handleARFaceScale = (event: Event, newValue: number | number[]) => {
    setARFaceScale(newValue as number)
    controlARFace(ARFaceControlType.Scale, newValue as number)
  }

  return (
    <Grid container spacing={2}>
      <Grid item xs={10}>
        <p> 2D Avatar Loader </p>
      </Grid>
      <Grid item xs={10}>
        <Select options={avatarOptions} onChange={(item) => { if (item != null) { setSelectedToken(item!.value) } }} />
        <VideoDeviceSelect />
      </Grid>
      <Grid item xs={5}>
        <ButtonGroup variant="contained" aria-label="outlined primary button group">
          <Button onClick={(item) => loadAvatar(templates[0].id, templates[0].assetUri, tokens[selectedToken].tokenId, tokens[selectedToken].assetUri)}>Load Avatar</Button>
          <Button onClick={(item) => switchARMode()}>Switch AR Mode</Button>
          <Button onClick={(item) => selectAvatar(templates[0].id, tokens[selectedToken].tokenId)}>Select Avatar</Button>
          <Button onClick={(item) => captureImage(isARMode)}>Capture</Button>
          <Button onClick={(item) => startRecordingVideo(isARMode, 60)}>Start Recording</Button>
          <Button onClick={(item) => stopRecordingVideo()}>Stop Recording</Button>
        </ButtonGroup>
      </Grid>
      <Grid item xs={5}>
        <MYTYSDKView />
      </Grid>
      <Grid item xs={5}>
        <Typography>
          Synced Blink Slider
        </Typography>
        <Slider
          value={syncedBlinkScale}
          onChange={handleSyncedBlinkSlider}
          step={1}
          min={0}
          max={100}
        />
        <Typography>
          Blink Slider
        </Typography>
        <Slider
          value={blinkScale}
          onChange={handleBlinkSlider}
          step={1}
          min={0}
          max={200}
        />
        <Typography>
          Eyebrow Slider
        </Typography>
        <Slider
          value={eyebrowScale}
          onChange={handleEyebrowSlider}
          step={1}
          min={0}
          max={200}
        />
        <Typography>
          Pupil Slider
        </Typography>
        <Slider
          value={pupilScale}
          onChange={handlePupilSlider}
          step={1}
          min={0}
          max={200}
        />
        <Typography>
          MouthX Slider
        </Typography>
        <Slider
          value={mouthXScale}
          onChange={handleMouthXSlider}
          step={1}
          min={0}
          max={200}
        />
        <Typography>
          MouthY Slider
        </Typography>
        <Slider
          value={mouthYScale}
          onChange={handleMouthYSlider}
          step={1}
          min={0}
          max={200}
        />
      </Grid>
      <Grid item xs={5}>
        <Typography>
          AR Face XOffset
        </Typography>
        <Slider
          value={arFaceXOffset}
          onChange={handleARFaceXOffset}
          step={0.01}
          min={-1}
          max={1}
        />
        <Typography>
          AR Face YOffset
        </Typography>
        <Slider
          value={arFaceYOffset}
          onChange={handleARFaceYOffset}
          step={0.01}
          min={-1}
          max={1}
        />
        <Typography>
          AR Face Scale
        </Typography>
        <Slider
          value={arFaceScale}
          onChange={handleARFaceScale}
          step={0.01}
          min={0.5}
          max={2}
        />
      </Grid>
    </Grid>
  );
}

export default ExampleView;
