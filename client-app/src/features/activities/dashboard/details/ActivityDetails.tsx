import React, {useContext, useEffect} from 'react'
import {Button, Card, Image} from "semantic-ui-react";
import {observer} from "mobx-react-lite";
import activityStore from "../../../../app/stores/activityStore";
import {RouteComponentProps} from 'react-router-dom';
import LoadingComponent from "../../../LoadingComponent";

interface DetailParams {
    id: string
}

const ActivityDetails: React.FC<RouteComponentProps<DetailParams>> = ({match, history}) => {
    const store = useContext(activityStore);
    const {activity, loadActivity, loadingInitial} = store;

    useEffect(() => {
        loadActivity(match.params.id);
    }, [loadActivity]);

    if (loadingInitial || !activity) return <LoadingComponent content='Loading activity...'/>

    return (
        <Card fluid>
            <Image src={`/assets/categoryImages/${activity.category}.jpg`} wrapped ui={false}/>
            <Card.Content>
                <Card.Header>{activity.title}</Card.Header>
                <Card.Meta>
                    <span>{activity.date}</span>
                </Card.Meta>
                <Card.Description>
                    {activity.description}
                </Card.Description>
            </Card.Content>
            <Card.Content extra>
                <Button.Group widths={2}>
                    <Button onClick={() => store.setEditMode(true)} basic color='blue' content='Edit'/>
                    <Button onClick={() => {
                        store.selectActivity();
                        history.push('/activities')
                    }} basic color='grey' content='Cancel'/>
                </Button.Group>
            </Card.Content>
        </Card>
    )
};

export default observer(ActivityDetails);