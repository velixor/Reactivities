import {IActivity} from "../../models/activity";
import {IUser} from "../../models/user";
import {IAttendee} from "../../models/attendee";

export const combineDateAndTime = (date: Date, time: Date) => {
    const timeString = time.getHours() + ':' + time.getMinutes() + ':00';

    const year = date.getFullYear();
    const month = date.getMonth() + 1;
    const day = date.getDate();
    const dateString = `${year}-${month}-${day}`;

    return new Date(dateString + ' ' + timeString);
}

export const setActivityProps = (activity: IActivity, user: IUser) => {
    activity.date = new Date(activity.date);

    const userAttendee = activity.attendees.find(x => x.userName === user.userName);
    if (userAttendee !== undefined) {
        activity.isGoing = true;
        activity.isHost = userAttendee.isHost;
    }
}

export const createAttendee = (user: IUser): IAttendee => {
    return {
        displayName: user.displayName,
        isHost: false,
        userName: user.userName,
        image: user.image!
    }
}