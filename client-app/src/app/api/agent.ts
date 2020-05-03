import axios, {AxiosResponse} from "axios";
import {IActivity} from "../models/activity";

axios.defaults.baseURL = 'http://localhost:5000/api';

const sleep = (ms: number) => (response: AxiosResponse) =>
    new Promise<AxiosResponse>(resolve => setTimeout(() => resolve(response), ms))

const responseBody = (response: AxiosResponse) => response.data;

const requests = {
    get: (url: string) => axios.get(url).then(sleep(1000)).then(responseBody),
    post: (url: string, body: {}) => axios.post(url, body).then(sleep(1000)).then(responseBody),
    put: (url: string, body: {}) => axios.put(url, body).then(sleep(1000)).then(responseBody),
    delete: (url: string) => axios.delete(url).then(sleep(1000)).then(responseBody)
};

const defaultPath = '/activities';
const getPath = (id?: string): string => {
    return id === undefined ? defaultPath : defaultPath + `/${id}`;
};

const Activities = {
    list: (): Promise<IActivity[]> => requests.get(getPath()),
    details: (id: string): Promise<IActivity> => requests.get(getPath(id)),
    create: (activity: IActivity) => requests.post(getPath(), activity),
    update: (activity: IActivity) => requests.put(getPath(activity.id), activity),
    delete: (id: string) => requests.delete(getPath(id))
};

export default {Activities};