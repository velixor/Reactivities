import React, {useContext} from 'react'
import {Button, Card, Image} from "semantic-ui-react";
import {IActivity} from "../../../../app/models/activity";
import {observer} from "mobx-react-lite";
import activityStore from "../../../../app/stores/activityStore";

interface IProps {
    activity: IActivity;
}

const ActivityDetails: React.FC<IProps> = ({activity}) => {
    const store = useContext(activityStore);

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
                    <Button onClick={() => store.selectActivity()} basic color='grey' content='Cancel'/>
                </Button.Group>
            </Card.Content>
        </Card>
    )
};

export default observer(ActivityDetails);