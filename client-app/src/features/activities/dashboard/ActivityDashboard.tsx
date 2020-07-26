import React, {useContext, useEffect} from 'react'
import {Grid} from "semantic-ui-react";
import ActivityList from "./ActivityList";
import {observer} from "mobx-react-lite";
import ActivityStore from "../../../app/stores/activityStore";
import LoadingComponent from "../../LoadingComponent";


const ActivityDashboard = () => {
    const activityStore = useContext(ActivityStore);

    useEffect(() => {
        activityStore.loadActivities();
    }, [activityStore]);

    if (activityStore.loadingInitial) return (<LoadingComponent content='Loading activities' inverted={true}/>);

    return (
        <Grid>
            <Grid.Column width={10}>
                <ActivityList/>
            </Grid.Column>
            <Grid.Column width={6}>
                <h2>(TODO) Activity filters</h2>
            </Grid.Column>
        </Grid>
    )
};

export default observer(ActivityDashboard)