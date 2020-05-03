import React, {Fragment, useContext, useEffect} from 'react';
import {Container} from 'semantic-ui-react';
import NavBar from "../../features/nav/NavBar";
import ActivityDashboard from "../../features/activities/dashboard/ActivityDashboard";
import LoadingComponent from "../../features/LoadingComponent";
import ActivityStore from "../stores/activityStore";
import {observer} from "mobx-react-lite";

const App = () => {
    const activityStore = useContext(ActivityStore);

    useEffect(() => {
        activityStore.loadActivities();
    }, [activityStore]);

    if (activityStore.loadingInitial) return (<LoadingComponent content='Loading activities' inverted={true}/>);

    return (
        <Fragment>
            <NavBar/>
            <Container style={{marginTop: '7em'}}>
                <ActivityDashboard/>
            </Container>
        </Fragment>
    );
};

export default observer(App);