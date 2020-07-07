import {action, computed, configure, observable, runInAction} from "mobx";
import {createContext} from "react";
import {IActivity} from "../models/activity";
import agent from "../api/agent";

configure({enforceActions: 'always'});

class activityStore {
    @observable activitiesRegistry = new Map();
    @observable activity: IActivity | undefined;
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
            runInAction(() => {
                activities.forEach((activity) => {
                    activity.date = activity.date.split('.')[0];
                    this.activitiesRegistry.set(activity.id, activity);
                });
            });
        } catch (e) {
            console.log(e);
        }
        runInAction(() => {
            this.loadingInitial = false;
        });
    };
    @action loadActivity = async (id: string) => {
        let activity = this.activitiesRegistry.get(id);
        if (activity) {
            this.activity = activity;
        } else {
            try {
                this.loadingInitial = true;
                activity = await agent.Activities.details(id);
                runInAction('getting activity', () => {
                    this.activity = activity;
                    this.loadingInitial = false;
                });
            } catch (error) {
                runInAction('get activity error', () => {
                    this.loadingInitial = false;
                });
                console.log(error);
            }
        }
    };
    @action createActivity = async (activity: IActivity) => {
        this.setSubmitting(true);
        await agent.Activities.create(activity);
        runInAction('creatingAction', () => {
            this.activitiesRegistry.set(activity.id, activity);
            this.selectActivity(activity);
            this.setEditMode(false);
            this.setSubmitting(false);
        });
    };
    @action deleteActivity = async (id: string) => {
        this.setTarget(id);
        this.setSubmitting(true);
        await agent.Activities.delete(id);
        runInAction('deletingAction', () => {
            this.activitiesRegistry.delete(id);
            if (this.activity && this.activity.id === id)
                this.selectActivity();
            this.setSubmitting(false);
        });
    };
    @action editActivity = async (activity: IActivity) => {
        this.setSubmitting(true);
        await agent.Activities.update(activity);
        runInAction('editingAction', () => {
            this.activitiesRegistry.set(activity.id, activity);
            this.selectActivity(activity);
            this.setEditMode(false);
            this.setSubmitting(false);
        });
    };

    @action selectActivity = (activity?: IActivity) => {
        this.activity = activity;
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