import React from 'react'
import {Item,Segment} from "semantic-ui-react";
import {IActivity} from "../../../app/models/activity";
import ActivityItem from "./ActivityItem";

interface IProps {
    activities: IActivity[];
    setSelectedActivity: (activity: IActivity) => void;
    deleteActivity: (id: string) => void;
}

const ActivityList: React.FC<IProps> = ({activities,setSelectedActivity,deleteActivity}) => {
    return (
        <Segment>
            <Item.Group divided>
                {activities.map(activity => (
                    <ActivityItem key={activity.id} activity={activity} setSelectedActivity={setSelectedActivity}
                                  deleteActivity={deleteActivity}/>
                ))}
            </Item.Group>
        </Segment>
    )
};

export default ActivityList