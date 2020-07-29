import React, {FormEvent, useContext, useEffect, useState} from 'react'
import {Button, Form, Grid, Segment} from "semantic-ui-react";
import {v4 as uuid} from 'uuid';
import activityStore from "../../../../app/stores/activityStore";
import {observer} from "mobx-react-lite";
import {RouteComponentProps} from 'react-router-dom';

interface DetailParams {
    id: string;
}

const ActivityForm: React.FC<RouteComponentProps<DetailParams>> = ({match, history}) => {
    const store = useContext(activityStore);

    const [activity, setActivity] = useState({
        id: '',
        title: '',
        description: '',
        category: '',
        date: '',
        city: '',
        venue: ''
    });

    useEffect(() => {
        if (match.params.id && activity.id.length === 0) {
            store.loadActivity(match.params.id).then(() => store.activity && setActivity(store.activity));
        }
        return () => {
            store.clearActivity();
        };
    }, [store.loadActivity, match.params.id, store.clearActivity, store.activity, activity.id.length, store]);

    const handleInputChange = (event: FormEvent<HTMLInputElement | HTMLTextAreaElement>) => {
        const {name, value} = event.currentTarget;
        setActivity({...activity, [name]: value});
    };

    const handleSubmit = async () => {
        const pushHistory = (activityId: string) => history.push(`/activities/${activityId}`);
        if (activity.id.length === 0) {
            let newActivity = {...activity, id: uuid()};
            await store.createActivity(newActivity).then(() => pushHistory(newActivity.id));
        } else
            await store.editActivity(activity).then(() => pushHistory(activity.id));
    };

    return (
        <Grid>
            <Grid.Column width={10}>
                <Segment clearing>
                    <Form onSubmit={handleSubmit}>
                        <Form.Input onChange={handleInputChange} name='title' placeholder='Title'
                                    value={activity.title}/>
                        <Form.TextArea onChange={handleInputChange} name='description' rows={2}
                                       placeholder='Description'
                                       value={activity.description}/>
                        <Form.Input onChange={handleInputChange} name='category' placeholder='Category'
                                    value={activity.category}/>
                        <Form.Input onChange={handleInputChange} name='date' type='datetime-local' placeholder='Date'
                                    value={activity.date}/>
                        <Form.Input onChange={handleInputChange} name='city' placeholder='City' value={activity.city}/>
                        <Form.Input onChange={handleInputChange} name='venue' placeholder='Venue'
                                    value={activity.venue}/>
                        <Button loading={store.submitting} floated='right' positive type='submit' content='Submit'/>
                        <Button onClick={() => history.push('/activities')} floated='right' type='button'
                                content='Cancel'/>
                    </Form>
                </Segment>
            </Grid.Column>
        </Grid>
    )
};

export default observer(ActivityForm);