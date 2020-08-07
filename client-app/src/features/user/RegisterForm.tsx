import React, {useContext} from 'react';
import {Field, Form as FinalForm} from 'react-final-form';
import TextInput from "../../app/common/form/TextInput";
import {Button, Form, Header, Label} from 'semantic-ui-react';
import {RootStoreContext} from "../../app/stores/rootStore";
import {IUserFormValues} from "../../app/models/user";
import {FORM_ERROR} from "final-form";
import {combineValidators, isRequired} from "revalidate";
import {history} from "../../index";
import ErrorMessage from "../../app/common/form/ErrorMessage";

const validate = combineValidators({
    username: isRequired('username'),
    displayName: isRequired('display name'),
    email: isRequired('email'),
    password: isRequired('password')
})
const RegisterForm = () => {
    const rootStore = useContext(RootStoreContext);
    const {register, isLoggedIn} = rootStore.userStore;

    if (isLoggedIn) {
        history.push('/activities');
    }
    return (
        <FinalForm
            onSubmit={(values: IUserFormValues) => register(values).catch(error => ({
                [FORM_ERROR]: error
            }))}
            validate={validate}
            render={({handleSubmit, submitting, submitError, invalid, pristine, dirtySinceLastSubmit}) => (
                <Form onSubmit={handleSubmit} error>
                    <Header as='h2' content='Sign up to Reactivities' color='teal' textAlign='center'/>
                    <Field name='username' component={TextInput} placeholder='Username'/>
                    <Field name='email' component={TextInput} placeholder='Email'/>
                    <Field name='displayName' component={TextInput} placeholder='Display Name'/>
                    <Field name='password' component={TextInput} placeholder='Password' type='password'/>
                    {submitError && !dirtySinceLastSubmit &&
                    <ErrorMessage error={submitError} text='Invalid email or password'/>}
                    <Button disabled={invalid && !dirtySinceLastSubmit || pristine} loading={submitting} positive
                            content='Register' fluid color='teal'/>
                </Form>
            )}
        />
    );
}

export default RegisterForm;