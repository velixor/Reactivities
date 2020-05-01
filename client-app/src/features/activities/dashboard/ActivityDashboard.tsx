import React from 'react'
import {Grid} from "semantic-ui-react";
import {IActivity} from "../../../app/models/activity";
import ActivityDetails from "./details/ActivityDetails";
import ActivityForm from "./form/ActivityForm";
import ActivityList from "./ActivityList";

interface IProps {
    activities: IActivity[];
    selectedActivity: IActivity | null;
    editMode: boolean;
    setSelectedActivity: (activity: IActivity | null) => void;
    setEditMode: (editMode: boolean) => void;
    createActivity: (activity: IActivity) => void;
    editActivity: (activity: IActivity) => void;
    deleteActivity: (id: string) => void;
}

const ActivityDashboard: React.FC<IProps> = (p) => {
    return (
        <Grid>
            <Grid.Column width={10}>
                <ActivityList activities={p.activities} setSelectedActivity={p.setSelectedActivity}
                              deleteActivity={p.deleteActivity}/>
            </Grid.Column>
            <Grid.Column width={6}>
                {p.editMode
                    ? <ActivityForm key={p.selectedActivity ? p.selectedActivity.id : 0} setEditMode={p.setEditMode}
                                    activity={p.selectedActivity!}
                                    createActivity={p.createActivity} editActivity={p.editActivity}/>
                    : p.selectedActivity && <ActivityDetails activity={p.selectedActivity}
                                                             setEditMode={p.setEditMode}
                                                             setSelectedActivity={p.setSelectedActivity}/>}

            </Grid.Column>
        </Grid>
    )
};

export default ActivityDashboard