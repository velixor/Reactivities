import React from 'react'
import {Item,Segment} from "semantic-ui-react";
import {IActivity} from "../../../app/models/activity";
import ActivityItem from "./ActivityItem";

interface IProps {
    activities: IActivity[]
}

const ActivityList: React.FC<IProps> = ({activities}) => {
    return (
        <Segment>
            <Item.Group divided>
                {activities.map(activity => (
                    <ActivityItem activity={activity}/>
                ))}
            </Item.Group>
        </Segment>
    )
};

export default ActivityList