import React, {Component} from 'react';
import './App.css';
import axios from 'axios';
import {Header, Icon, List} from "semantic-ui-react";

class App extends Component {
    state = {
        values: []
    };

    componentDidMount(): void {
        axios.get("http://localhost:5000/api/values")
            .then((response) => {
                console.log(response);
                this.setState({
                    values: response.data
                });
            });
    }

    render() {
        return (
            <div>
                <Header as='h2' icon textAlign='center'>
                    <Icon name='users' circular/>
                    <Header.Content>Friends</Header.Content>
                </Header>
                <List>
                    {this.state.values.map((value: any) => (
                        <List.Item key={value.id}>{value.name}</List.Item>
                    ))}
                </List>
            </div>
        );
    }
}

export default App;