import {RootStore} from "./rootStore";
import {action, observable} from "mobx";
import { jwtKey } from "../common/constants";

export default class CommonStore {
    rootStore: RootStore;

    constructor(rootStore: RootStore) {
        this.rootStore = rootStore;
    }

    @observable token: string | null = null;
    @observable appLoaded = false;

    @action setToken = (token: string | null) => {
        if (token) {
            window.localStorage.setItem(jwtKey, token);
            this.token = token;
        } else {
            window.localStorage.removeItem(jwtKey)
        }
    }
    @action setAppLoaded = () => {
        this.appLoaded = true;
    }
}