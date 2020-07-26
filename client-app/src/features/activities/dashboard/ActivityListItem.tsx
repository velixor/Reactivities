import React from 'react'
import {IActivity} from "../../../app/models/activity";
import {Button, Item, Label} from "semantic-ui-react";
import {observer} from "mobx-react-lite";
import {Link} from "react-router-dom";

interface IProps {
    activity: IActivity;
}

const ActivityListItem: React.FC<IProps> = ({activity}) => {
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
                        as={Link} to={`activities/${activity.id}`}
                        floated='right'
                        content='View'
                        color='blue'/>
                    <Label basic content={activity.category}/>
                </Item.Extra>
            </Item.Content>
        </Item>
    )
};

export default observer(ActivityListItem);