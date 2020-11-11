import {RootStore} from "./rootStore";
import {action, computed, observable, runInAction} from "mobx";
import {IPhoto, IProfile} from "../models/profile";
import agent from "../api/agent";
import {toast} from "react-toastify";

export default class ProfileStore {
    rootStore: RootStore

    constructor(rootStore: RootStore) {
        this.rootStore = rootStore;
    }

    @observable profile: IProfile | null = null;
    @observable loadingProfile = false;
    @observable uploadingPhoto = false;
    @observable settingMainPhotoId: string | null = null;
    @observable deletingPhotoId: string | null = null;

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

    @action uploadPhoto = async (file: Blob) => {
        this.uploadingPhoto = true;
        try {
            const photo = await agent.Profiles.uploadPhoto(file);
            runInAction(() => {
                if (this.profile) {
                    this.profile.photos.push(photo);
                    if (photo.isMain && this.rootStore.userStore.user) {
                        this.rootStore.userStore.user.image = photo.url;
                        this.profile.image = photo.url;
                    }
                }
            })
        } catch (error) {
            console.log(error);
            toast.error('Problem uploading photo');
        } finally {
            runInAction('finish uploading photo', () => {
                this.uploadingPhoto = false;
            })
        }
    }

    @action setMainPhoto = async (photo: IPhoto) => {
        this.settingMainPhotoId = photo.id;
        try {
            await agent.Profiles.setMainPhoto(photo.id);
            runInAction('setting main photo', () => {
                this.rootStore.userStore.user!.image = photo.url;

                this.profile!.photos.find(x => x.isMain)!.isMain = false;
                this.profile!.photos.find(x => x.id === photo.id)!.isMain = true;

                this.profile!.image = photo.url;
            });
        } catch (error) {
            console.log(error);
            toast.error('Problem with setting main photo');
        } finally {
            runInAction('finish setting main photo', () => {
                this.settingMainPhotoId = null;
            });
        }
    }

    @action deletePhoto = async (photo: IPhoto) => {
        if (this.profile!.photos.find(x => x.isMain)!.id === photo.id) {
            toast.warn("You can't delete main photo");
            return;
        }

        this.deletingPhotoId = photo.id;

        try {
            await agent.Profiles.deletePhoto(photo.id);
            runInAction('deleting photo', () => {
                this.profile!.photos = this.profile!.photos.filter(x => x.id !== photo.id);
            });
        } catch (error) {
            console.log(error);
            toast.error('Problem with photo deleting');
        } finally {
            runInAction('finish deleting photo', () => {
                this.deletingPhotoId = null;
            })
        }
    }
}