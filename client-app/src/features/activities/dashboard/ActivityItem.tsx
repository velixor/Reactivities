import React, {SyntheticEvent} from 'react'
import {IActivity} from "../../../app/models/activity";
import {Button, Item, Label} from "semantic-ui-react";

interface IProps {
    activity: IActivity;
    setSelectedActivity: (activity: IActivity) => void;
    deleteActivity: (event: SyntheticEvent<HTMLButtonElement>, id: string) => void;
    submitting: boolean;
    target: string;
}

const ActivityItem: React.FC<IProps> = (p) => {
    return (
        <Item>
            <Item.Content>
                <Item.Header as='a'>{p.activity.title}</Item.Header>
                <Item.Meta>{p.activity.date}</Item.Meta>
                <Item.Description>
                    <div>{p.activity.description}</div>
                    <div>{p.activity.city}, {p.activity.venue}</div>
                </Item.Description>
                <Item.Extra>
                    <Button
                        onClick={() => p.setSelectedActivity(p.activity)}
                        floated='right'
                        content='View'
                        color='blue'/>
                    <Button name={p.activity.id} loading={p.submitting && p.target == p.activity.id}
                            onClick={(e) => p.deleteActivity(e, p.activity.id)}
                            floated='right'
                            content='Delete' color='red'/>
                    <Label basic content={p.activity.category}/>
                </Item.Extra>
            </Item.Content>
        </Item>
    )
};

export default ActivityItem