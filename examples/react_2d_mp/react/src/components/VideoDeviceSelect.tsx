import { useContext } from "react"
import Select from 'react-select';
import { VideoDeviceContext, videoDeviceContext } from "../context/VideoDeviceContext"

const VideoDeviceSelect = () => {
  const { deviceOptions, onCameraChanged } = useContext(videoDeviceContext) as VideoDeviceContext;

  return <Select options={deviceOptions} onChange={(item) => onCameraChanged(item!.value, item!.label)}/>
}

export default VideoDeviceSelect;