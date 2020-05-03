import React, {Fragment, SyntheticEvent, useEffect, useState} from 'react';
import {Container} from 'semantic-ui-react';
import {IActivity} from "../models/activity";
import NavBar from "../../features/nav/NavBar";
import ActivityDashboard from "../../features/activities/dashboard/ActivityDashboard";
import agent from "../api/agent";
import LoadingComponent from "../../features/LoadingComponent";

const App = () => {
    const [activities, setActivities] = useState<IActivity[]>([]);
    const [selectedActivity, setSelectedActivity] = useState<IActivity | null>(null);
    const [editMode, setEditMode] = useState(false);
    const [loading, setLoading] = useState(true);
    const [submitting, setSubmitting] = useState(false);
    const [target, setTarget] = useState('');

    useEffect(() => {
        agent.Activities.list()
             .then((response) => {
                 let activities = response;
                 activities.forEach((activity) => {
                     activity.date = activity.date.split('.')[0];
                 });
                 setActivities(activities);
             })
             .then(() => setLoading(false));
    }, []);

    const handleSelectActivity = (activity: IActivity | null) => {
        if (activity)
            setSelectedActivity(activity);
        setEditMode(false);
    };
    const handleOpenCreateActivityForm = () => {
        setSelectedActivity(null);
        setEditMode(true);
    };
    const handleCreateActivity = (activity: IActivity) => {
        setSubmitting(true);
        agent.Activities.create(activity)
             .then(() => {
                 setActivities([...activities, activity]);
                 setSelectedActivity(activity);
                 setEditMode(false);
             })
             .then(() => setSubmitting(false));
    };
    const handleEditActivity = (activity: IActivity) => {
        setSubmitting(true);
        agent.Activities.update(activity)
             .then(() => {
                 setActivities([...activities.filter(a => a.id !== activity.id), activity]);
                 setSelectedActivity(activity);
                 setEditMode(false);
             })
             .then(() => setSubmitting(false));
    };

    const handleDeleteActivity = (event: SyntheticEvent<HTMLButtonElement>, id: string) => {
        setTarget(event.currentTarget.name);
        setSubmitting(true);
        agent.Activities.delete(id)
             .then(() => {
                 setActivities([...activities.filter(a => a.id !== id)]);
                 if (selectedActivity && selectedActivity.id === id)
                     setSelectedActivity(null);
             })
             .then(() => setSubmitting(false));
    };

    if (loading) return (<LoadingComponent content='Loading activities' inverted={true}/>);

    return (
        <Fragment>
            <NavBar openCreateActivityForm={handleOpenCreateActivityForm}/>
            <Container style={{marginTop: '7em'}}>
                <ActivityDashboard
                    activities={activities}
                    selectedActivity={selectedActivity}
                    editMode={editMode}
                    setSelectedActivity={handleSelectActivity}
                    setEditMode={setEditMode}
                    createActivity={handleCreateActivity}
                    editActivity={handleEditActivity}
                    deleteActivity={handleDeleteActivity}
                    submitting={submitting}
                    target={target}
                />
            </Container>
        </Fragment>
    );
};

export default App;