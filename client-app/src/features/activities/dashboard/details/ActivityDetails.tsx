import React, {useContext, useEffect} from 'react'
import {Grid} from "semantic-ui-react";
import {observer} from "mobx-react-lite";
import activityStore from "../../../../app/stores/activityStore";
import {RouteComponentProps} from 'react-router-dom';
import LoadingComponent from "../../../LoadingComponent";
import ActivityDetailedSidebar from "./ActivityDetailedSidebar";
import ActivityDetailedHeader from "./ActivityDetailedHeader";
import ActivityDetailedInfo from "./ActivityDetailedInfo";
import ActivityDetailedChat from "./ActivityDetailedChat";

interface DetailParams {
    id: string
}

const ActivityDetails: React.FC<RouteComponentProps<DetailParams>> = ({match, history}) => {
    const store = useContext(activityStore);
    const {activity, loadActivity, loadingInitial} = store;

    useEffect(() => {
        loadActivity(match.params.id);
    }, [loadActivity, match.params.id]);

    if (loadingInitial) return <LoadingComponent content='Loading activity...'/>
    if (!activity) return <h1>Not found</h1>

    return (
        <Grid>
            <Grid.Column width={10}>
                <ActivityDetailedHeader activity={activity}/>
                <ActivityDetailedInfo activity={activity}/>
                <ActivityDetailedChat/>
            </Grid.Column>
            <Grid.Column width={6}>
                <ActivityDetailedSidebar/>
            </Grid.Column>
        </Grid>
    )
};

export default observer(ActivityDetails);