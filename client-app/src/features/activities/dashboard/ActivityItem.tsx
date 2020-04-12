import React from 'react'
import {IActivity} from "../../../app/models/activity";
import {Button,Item,Label} from "semantic-ui-react";

interface IProps {
    activity: IActivity;
    setSelectedActivity: (activity: IActivity) => void;
    deleteActivity: (id: string) => void;
}

const ActivityItem: React.FC<IProps> = ({activity,setSelectedActivity,deleteActivity}) => {
    return (
        <Item>
            <Item.Content>
                <Item.Header as='a'>{activity.title}</Item.Header>
                <Item.Meta>{activity.date}</Item.Meta>
                <Item.Description>
                    <div>{activity.description}</div>
                    <div>{activity.city}, {activity.venue}</div>
                </Item.Description>
                <Item.Extra>
                    <Button
                        onClick={() => setSelectedActivity(activity)}
                        floated='right'
                        content='View'
                        color='blue'/>
                    <Button onClick={() => deleteActivity(activity.id)} floated='right' content='Delete' color='red'/>
                    <Label basic content={activity.category}/>
                </Item.Extra>
            </Item.Content>
        </Item>
    )
};

export default ActivityItem