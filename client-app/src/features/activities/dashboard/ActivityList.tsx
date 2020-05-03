import React, {useContext} from 'react'
import {Item, Segment} from "semantic-ui-react";
import ActivityItem from "./ActivityItem";
import {observer} from "mobx-react-lite";
import activityStore from "../../../app/stores/activityStore";


const ActivityList = () => {
    const store = useContext(activityStore);
    return (
        <Segment>
            <Item.Group divided>
                {store.activitiesByDate.map(activity => (
                    <ActivityItem key={activity.id} activity={activity}/>
                ))}
            </Item.Group>
        </Segment>
    )
};

export default observer(ActivityList)