import {action, computed, observable} from "mobx";
import {createContext} from "react";
import {IActivity} from "../models/activity";
import agent from "../api/agent";

class activityStore {
    @observable activitiesRegistry = new Map();
    @observable selectedActivity: IActivity | undefined;
    @observable loadingInitial = false;
    @observable editMode = false;
    @observable submitting = false;
    @observable target = '';

    @computed get activitiesByDate() {
        return Array.from(this.activitiesRegistry.values())
                    .sort((a, b) => Date.parse(a.date) - Date.parse(b.date));
    };

    @action loadActivities = async () => {
        try {
            this.loadingInitial = true;
            const activities = await agent.Activities.list();
            activities.forEach((activity) => {
                activity.date = activity.date.split('.')[0];
                this.activitiesRegistry.set(activity.id, activity);
            });
        } catch (e) {
            console.log(e);
        }
        this.loadingInitial = false;
    };
    @action createActivity = async (activity: IActivity) => {
        this.setSubmitting(true);
        await agent.Activities.create(activity);
        this.activitiesRegistry.set(activity.id, activity);
        this.selectActivity(activity);
        this.setEditMode(false);
        this.setSubmitting(false);
    };
    @action deleteActivity = async (id: string) => {
        this.setTarget(id);
        this.setSubmitting(true);
        await agent.Activities.delete(id);
        this.activitiesRegistry.delete(id);
        if (this.selectedActivity && this.selectedActivity.id === id)
            this.selectActivity();
        this.setSubmitting(false);
    };
    @action editActivity = async (activity: IActivity) => {
        this.setSubmitting(true);
        await agent.Activities.update(activity)
        this.activitiesRegistry.delete(activity.id)
        this.activitiesRegistry.set(activity.id, activity);
        this.selectActivity(activity);
        this.setEditMode(false);
        this.setSubmitting(false);
    };

    @action selectActivity = (activity?: IActivity) => {
        this.selectedActivity = activity;
        this.editMode = false;
    };
    @action openCreateActivityForm = () => {
        this.selectActivity();
        this.setEditMode(true);
    };
    @action setEditMode = (editMode: boolean) => this.editMode = editMode;
    @action setSubmitting = (submitting: boolean) => this.submitting = submitting;
    @action setTarget = (target: string) => this.target = target;
}

export default createContext(new activityStore())