import React from 'react'
import {List, Image, Popup} from "semantic-ui-react";
import {IAttendee} from "../../../app/models/attendee";
import { Link } from 'react-router-dom';

interface IProps {
    attendees: IAttendee[];
}

const ActivityListItemAttendees: React.FC<IProps> = ({attendees}) => {
    return (
        <List horizontal>
            {attendees.map(attendee =>
                <List.Item key={attendee.userName}>
                    <Popup
                        header={attendee.displayName}
                        trigger={
                            <Image as={Link} to={`/profile/${attendee.userName}`} size='mini' circular src={attendee.image || '/assets/user.png'}/>
                        }/>
                </List.Item>
            )}
        </List>

    )
};

export default ActivityListItemAttendees