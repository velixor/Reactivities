import axios, {AxiosResponse} from "axios";
import {IActivity} from "../models/activity";

axios.defaults.baseURL = 'http://localhost:5000/api';

const responseBody = (response: AxiosResponse) => response.data;

const requests = {
    get: (url: string) => axios.get(url).then(responseBody),
    post: (url: string, body: {}) => axios.post(url, body).then(responseBody),
    put: (url: string, body: {}) => axios.put(url, body).then(responseBody),
    delete: (url: string) => axios.delete(url).then(responseBody)
};

const defaultPath = '/activities';
const getPath = (id?: string): string => {
    return id === undefined ? defaultPath : defaultPath + `/${id}`;
};

const Activities = {
    list: () => requests.get(getPath()),
    details: (id: string) => requests.get(getPath(id)),
    create: (activity: IActivity) => requests.post(getPath(), activity),
    update: (id: string, activity: IActivity) => requests.put(getPath(id), activity),
    delete: (id: string) => requests.delete(getPath(id))
};

export default Activities;