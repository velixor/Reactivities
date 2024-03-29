import React, {useCallback} from 'react'
import {useDropzone} from 'react-dropzone'
import {Header, Icon} from "semantic-ui-react";

const dropzoneStyles = {
    border: 'dashed 3px',
    borderColor: '#eee',
    borderRadius: '5px',
    paddingTop: '30px',
    textAlign: 'center' as 'center',
    height: '200px'
};

const dropzoneActiveStyles = {
    borderColor: 'green'
};

interface IProps {
    setFiles: (file: object[]) => void
}

const PhotoWidgetDropzone: React.FC<IProps> = ({setFiles}) => {
    const onDrop = useCallback(acceptedFiles => {
        const filesWithPreview = acceptedFiles.map((file: object) => Object.assign(file, {
            preview: URL.createObjectURL(file)
        }));
        setFiles(filesWithPreview);
    }, [setFiles])
    const {getRootProps, getInputProps, isDragActive} = useDropzone({onDrop})

    return (
        <div
            {...getRootProps()}
            style={isDragActive
                ? {...dropzoneStyles, ...dropzoneActiveStyles}
                : dropzoneStyles}
        >
            <input {...getInputProps()} />
            <Icon name='upload' size='huge'/>
            <Header content='Drop image here'/>
        </div>
    )
}

export default PhotoWidgetDropzone;