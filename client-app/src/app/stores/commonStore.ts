import {RootStore} from "./rootStore";
import {action, observable, reaction} from "mobx";
import {jwtKey} from "../common/constants";

export default class CommonStore {
    rootStore: RootStore;

    constructor(rootStore: RootStore) {
        this.rootStore = rootStore;

        reaction(
            () => this.token,
            token => {
                if (token) {
                    window.localStorage.setItem(jwtKey, token);
                } else {
                    window.localStorage.removeItem(jwtKey);
                }
            }
        )
    }

    @observable token: string | null = window.localStorage.getItem(jwtKey);
    @observable appLoaded = false;

    @action setToken = (token: string | null) => {
        this.token = token;
    }
    @action setAppLoaded = () => {
        this.appLoaded = true;
    }
}