import React, {Fragment, useEffect, useState} from 'react';
import {Button, Grid, Header} from 'semantic-ui-react';
import {observer} from 'mobx-react-lite';
import PhotoWidgetDropzone from "./PhotoWidgetDropzone";
import PhotoWidgetCropper from "./PhotoWidgetCropper";

interface IProps {
    uploadPhoto: (photo: Blob) => void;
    loading: boolean;
}

const PhotoUploadWidget: React.FC<IProps> = ({uploadPhoto, loading}) => {
    const [files, setFiles] = useState<any[]>([]);
    const [image, setImage] = useState<Blob | null>(null);

    useEffect(() => {
        return () => {
            files.forEach((file) => URL.revokeObjectURL(file.preview));
        };
    });

    const getFile = (): any | null => {
        if (files.length === 0) return null;
        return files[0];
    }

    return (
        <Fragment>
            <Grid>
                <Grid.Column width={4}>
                    <Header color='teal' sub content='Step 1 - Add Photo'/>
                    <PhotoWidgetDropzone setFiles={setFiles}/>
                </Grid.Column>
                <Grid.Column width={1}/>
                <Grid.Column width={4}>
                    <Header sub color='teal' content='Step 2 - Resize image'/>
                    {getFile() && <PhotoWidgetCropper setImage={setImage} imagePreview={getFile().preview}/>}
                </Grid.Column>
                <Grid.Column width={1}/>
                <Grid.Column width={4}>
                    <Header sub color='teal' content='Step 3 - Preview & Upload'/>
                    {getFile() &&
                    <Fragment>
                        <div className='img-preview' style={{minHeight: '200px', overflow: 'hidden'}}/>
                        <Button.Group widths={2}>
                            <Button
                                positive
                                icon='check'
                                loading={loading}
                                onClick={() => uploadPhoto(image!)}
                            />
                            <Button
                                icon='close'
                                disabled={loading}
                                onClick={() => setFiles([])}
                            />
                        </Button.Group>
                    </Fragment>
                    }
                </Grid.Column>
            </Grid>
        </Fragment>
    );
};

export default observer(PhotoUploadWidget);