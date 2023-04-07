import '../App.css';
import { useEffect, useState, useContext } from 'react';
import ButtonGroup from '@mui/material/ButtonGroup';
import Select from 'react-select';
import Button from '@mui/material/Button';
import Grid from '@mui/material/Grid';
import { MYTYSDKContext, mytySDKContext} from "../context/MYTYSDKContext"
import VideoDeviceSelect from './VideoDeviceSelect';
import MYTYSDKView from './MYTYSDKView';
import useCaptureRecord from '../hooks/useCaptureRecord';

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
  const { loadAvatar, selectAvatar, setARMode } = useContext(mytySDKContext) as MYTYSDKContext
  const { captureImage, stopRecordingVideo, startRecordingVideo } = useCaptureRecord();
  const [templates, setTemplates] = useState<AssetVersion[]>([]);
  const [tokens, setTokens] = useState<Asset[]>([]);
  const [selectedToken, setSelectedToken] = useState<number>(0);
  const [isARMode, setIsARMode] = useState<boolean>(false);

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

  function changeARMode(flag: boolean)
  {
    setIsARMode(flag);
    setARMode(flag ? "true" : "false");
  }

  const avatarOptions = tokens.map((token, idx) => ({ value: idx, label: templates[0].description + token.tokenId }))

  useEffect(() => {
    fetchTemplates()
    fetchTokens()
  }, [])

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
          <Button onClick={(item) => changeARMode(true)}>Set AR Mode</Button>
          <Button onClick={(item) => changeARMode(false)}>Disable AR Mode</Button>
          <Button onClick={(item) => selectAvatar(templates[0].id, tokens[selectedToken].tokenId)}>Select Avatar</Button>
          <Button onClick={(item) => captureImage(isARMode)}>Capture</Button>
          <Button onClick={(item) => startRecordingVideo(isARMode, 60)}>Start Recording</Button>
          <Button onClick={(item) => stopRecordingVideo()}>Stop Recording</Button>
        </ButtonGroup>
      </Grid>
      <Grid item xs={5}>
        <MYTYSDKView />
      </Grid>
    </Grid>
  );
}

export default ExampleView;
