import React, {useContext} from 'react'
import {Button, Container, Menu} from "semantic-ui-react";
import {observer} from "mobx-react-lite";
import activityStore from "../../app/stores/activityStore";
import {NavLink} from "react-router-dom";

const NavBar = () => {
    const store = useContext(activityStore);

    return (
        <Menu fixed='top' inverted>
            <Container>
                <Menu.Item header as={NavLink} exact to='/'>
                    <img src="assets/logo.png" alt='logo' style={{marginRight: '10px'}}/>
                    Reactivities
                </Menu.Item>
                <Menu.Item name='Activities' as={NavLink} to='/activities'/>
                <Menu.Item>
                    <Button as={NavLink} to='/createActivity' onClick={() => store.openCreateActivityForm()} positive
                            content='Create Activity'/>
                </Menu.Item>
            </Container>
        </Menu>
    );
};

export default observer(NavBar)