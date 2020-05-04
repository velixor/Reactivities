import React, {useContext} from 'react'
import {IActivity} from "../../../app/models/activity";
import {Button, Item, Label} from "semantic-ui-react";
import activityStore from "../../../app/stores/activityStore";
import {observer} from "mobx-react-lite";
import {Link} from "react-router-dom";

interface IProps {
    activity: IActivity;
}

const ActivityItem: React.FC<IProps> = ({activity}) => {
    const store = useContext(activityStore);
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
                    <Button name={activity.id} loading={store.submitting && store.target === activity.id}
                            onClick={() => store.deleteActivity(activity.id)}
                            floated='right'
                            content='Delete' color='red'/>
                    <Label basic content={activity.category}/>
                </Item.Extra>
            </Item.Content>
        </Item>
    )
};

export default observer(ActivityItem);