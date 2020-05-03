import React from "react";
import {Dimmer, Loader} from "semantic-ui-react";

interface IProps {
    inverted?: boolean;
    content?: string;
}

const LoadingComponent: React.FC<IProps> = (props) => {
    return (
        <Dimmer active inverted={props.inverted}>
            <Loader content={props.content}/>
        </Dimmer>
    );
};

export default LoadingComponent;