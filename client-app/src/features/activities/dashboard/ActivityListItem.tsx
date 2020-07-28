import React from 'react'
import {IActivity} from "../../../app/models/activity";
import {Button, Icon, Item, Segment} from "semantic-ui-react";
import {observer} from "mobx-react-lite";
import {Link} from "react-router-dom";

interface IProps {
    activity: IActivity;
}

const ActivityListItem: React.FC<IProps> = ({activity}) => {
    return (
        <Segment.Group>
            <Segment>
                <Item.Group>
                    <Item>
                        <Item.Image size='tiny' circular src='/assets/user.png'/>
                        <Item.Content>
                            <Item.Header as='a'>{activity.title}</Item.Header>
                            <Item.Description>
                                Hosted by Fake
                            </Item.Description>
                        </Item.Content>
                    </Item>
                </Item.Group>
            </Segment>
            <Segment>
                <Icon name='clock'/> {activity.date}
                <Icon name='marker'/> {activity.venue}, {activity.city}
            </Segment>
            <Segment secondary>
                Attendees will go here
            </Segment>
            <Segment clearing>
                <span>{activity.description}</span>
                <Button
                    as={Link} to={`activities/${activity.id}`}
                    floated='right'
                    content='View'
                    color='blue'/>
            </Segment>
        </Segment.Group>
    )
};

export default observer(ActivityListItem);