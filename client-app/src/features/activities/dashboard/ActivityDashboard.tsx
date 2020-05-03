import React, {useContext} from 'react'
import {Grid} from "semantic-ui-react";
import ActivityDetails from "./details/ActivityDetails";
import ActivityForm from "./form/ActivityForm";
import ActivityList from "./ActivityList";
import {observer} from "mobx-react-lite";
import activityStore from "../../../app/stores/activityStore";


const ActivityDashboard = () => {
    const store = useContext(activityStore)
    return (
        <Grid>
            <Grid.Column width={10}>
                <ActivityList/>
            </Grid.Column>
            <Grid.Column width={6}>
                {store.editMode
                    ? <ActivityForm key={store.selectedActivity ? store.selectedActivity.id : 0}/>
                    : store.selectedActivity && <ActivityDetails activity={store.selectedActivity}/>}
            </Grid.Column>
        </Grid>
    )
};

export default observer(ActivityDashboard)