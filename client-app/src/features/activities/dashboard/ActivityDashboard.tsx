import React, {SyntheticEvent} from 'react'
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
    deleteActivity: (event: SyntheticEvent<HTMLButtonElement>, id: string) => void;
    submitting: boolean;
    target: string
}

const ActivityDashboard: React.FC<IProps> = (p) => {
    return (
        <Grid>
            <Grid.Column width={10}>
                <ActivityList activities={p.activities} setSelectedActivity={p.setSelectedActivity}
                              deleteActivity={p.deleteActivity} submitting={p.submitting} target={p.target}/>
            </Grid.Column>
            <Grid.Column width={6}>
                {p.editMode
                    ? <ActivityForm key={p.selectedActivity ? p.selectedActivity.id : 0} setEditMode={p.setEditMode}
                                    activity={p.selectedActivity!}
                                    createActivity={p.createActivity} editActivity={p.editActivity}
                                    submitting={p.submitting}/>
                    : p.selectedActivity && <ActivityDetails activity={p.selectedActivity}
                                                             setEditMode={p.setEditMode}
                                                             setSelectedActivity={p.setSelectedActivity}/>}

            </Grid.Column>
        </Grid>
    )
};

export default ActivityDashboard