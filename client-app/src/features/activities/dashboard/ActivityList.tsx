import React, {SyntheticEvent} from 'react'
import {Item, Segment} from "semantic-ui-react";
import {IActivity} from "../../../app/models/activity";
import ActivityItem from "./ActivityItem";

interface IProps {
    activities: IActivity[];
    setSelectedActivity: (activity: IActivity) => void;
    deleteActivity: (event: SyntheticEvent<HTMLButtonElement>, id: string) => void;
    submitting: boolean;
    target: string;
}

const ActivityList: React.FC<IProps> = (p) => {
    return (
        <Segment>
            <Item.Group divided>
                {p.activities.map(activity => (
                    <ActivityItem target={p.target} key={activity.id} activity={activity}
                                  setSelectedActivity={p.setSelectedActivity}
                                  deleteActivity={p.deleteActivity} submitting={p.submitting}/>
                ))}
            </Item.Group>
        </Segment>
    )
};

export default ActivityList