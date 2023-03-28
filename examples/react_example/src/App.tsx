import './App.css';
import { useEffect, useState } from 'react';
import { Unity, useUnityContext } from 'react-unity-webgl';
import ButtonGroup from '@mui/material/ButtonGroup';
import Select from 'react-select';
import Button from '@mui/material/Button';
import Grid from '@mui/material/Grid';
import { Container, TextField } from '@mui/material';

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
  const { unityProvider, isLoaded, sendMessage } = useUnityContext({
    loaderUrl: "WebGL/Build/WebGL.loader.js",
    dataUrl: "WebGL/Build/WebGL.data",
    frameworkUrl: "WebGL/Build/WebGL.framework.js",
    codeUrl: "WebGL/Build/WebGL.wasm",
  });

  const [templates, setTemplates] = useState<AssetVersion[]>([]);
  const [tokens, setTokens] = useState<Asset[]>([]);
  const [options, setOptions] = useState([]);
  const [selectedToken, setSelectedToken] = useState<number>(0);

  const fetchTemplates = () => {
    // fetch("https://api-dev.myty.space/asset/versions")
    //   .then(response => {
    //     return response.json()
    //   })
    //   .then(data => {
    //     setTemplates(data)
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

  return (
    <div className="App">
      <Grid container spacing={2}>
        <Grid item xs={10}>
          <p> Demo </p>
        </Grid>
        <Grid item xs={6}>
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
      </Grid>
    </div>
  );
}

export default App;
