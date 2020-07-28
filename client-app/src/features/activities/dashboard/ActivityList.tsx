import React, {Fragment, useContext} from 'react'
import {Item, Label} from "semantic-ui-react";
import ActivityItem from "./ActivityListItem";
import {observer} from "mobx-react-lite";
import activityStore from "../../../app/stores/activityStore";


const ActivityList = () => {
    const store = useContext(activityStore);
    return <Fragment>
        {store.activitiesByDate.map(([group, activities]) => (
            <Fragment key={group}>
                <Label size='large' color='blue'>
                    {group}
                </Label>
                <Item.Group divided>
                    {activities.map(activity => <ActivityItem key={activity.id} activity={activity}/>)}
                </Item.Group>
            </Fragment>
        ))}
    </Fragment>
};

export default observer(ActivityList);