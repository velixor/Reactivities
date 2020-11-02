import {RootStore} from "./rootStore";
import {action, computed, observable, runInAction} from "mobx";
import {IProfile} from "../models/profile";
import agent from "../api/agent";

export default class ProfileStore {
    rootStore: RootStore

    constructor(rootStore: RootStore) {
        this.rootStore = rootStore;
    }

    @observable profile: IProfile | null = null;
    @observable loadingProfile = false;

    @computed get isCurrentUser() {
        const profile = this.profile;
        const user = this.rootStore.userStore.user;

        if (profile && user) {
            return profile.userName === user.userName;
        } else {
            return false;
        }
    }

    @action loadProfile = async (userName: string) => {
        this.loadingProfile = true;
        try {
            const profile = await agent.Profiles.get(userName);
            runInAction('loading profile', () => {
                this.profile = profile;
            })
        } catch (error) {
            console.log(error);
        } finally {
            runInAction('finally loading profile', () => {
                this.loadingProfile = false;
            })
        }
    }
}