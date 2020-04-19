import React, {Fragment, useEffect, useState} from 'react';
import {Container} from 'semantic-ui-react';
import {IActivity} from "../models/activity";
import NavBar from "../../features/nav/NavBar";
import ActivityDashboard from "../../features/activities/dashboard/ActivityDashboard";
import Activities from "../api/agent";

const App = () => {
    const [activities, setActivities] = useState<IActivity[]>([]);
    const [selectedActivity, setSelectedActivity] = useState<IActivity | null>(null);
    const [editMode, setEditMode] = useState(false);

    useEffect(() => {
        Activities.list()
                  .then(response => {
                      let activities: IActivity[] = [];
                      [...response].forEach((a: any) => {
                          a.date = a.date.split('.')[0];
                          activities.push(a);
                      });
                      setActivities(activities);
                  });
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
        setActivities([...activities, activity]);
        setSelectedActivity(activity);
        setEditMode(false);
    };
    const handleEditActivity = (activity: IActivity) => {
        setActivities([...activities.filter(a => a.id !== activity.id), activity]);
        setSelectedActivity(activity);
        setEditMode(false);
    };

    const handleDeleteActivity = (id: string) => {
        setActivities([...activities.filter(a => a.id !== id)]);
    };

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
                />
            </Container>
        </Fragment>
    );
};

export default App;