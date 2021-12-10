import React from "react";
import {Button, Card, CardActions, CardContent, CircularProgress, Grid} from "@mui/material";
import PropTypes from 'prop-types';

TodoItem.propTypes = {
    id: PropTypes.number,
    date: PropTypes.any,
    state: PropTypes.string,
    title: PropTypes.string,
    desc: PropTypes.string,
    loading: PropTypes.bool,

    deleteHandler: PropTypes.func,
    moveHandler: PropTypes.func,
    editHandler: PropTypes.func
}

export function TodoItem(props) {
    function renderBody() {
        let dateString = (props.date != null) ?
            new Date(props.date).toLocaleDateString()
            : "No date specified"

        // capitalize first letter of state string
        let state = props.state
        state = state.charAt(0).toUpperCase() + state.slice(1)

        return (
            <div>
                <CardContent>
                    <Grid direction="column" container spacing={0}>
                        <Grid item>
                            <p className="item-name">{props.title}</p>
                        </Grid>
                        <Grid item>
                            <p className="item-desc">{props.desc}</p>
                        </Grid>
                        <Grid item container spacing={2}>
                            <Grid item xs={6}>
                                <span className="item-date">{dateString}</span>
                            </Grid>
                            <Grid item xs={6}>
                                <span className="item-state">{state}</span>
                            </Grid>
                        </Grid>
                    </Grid>
                </CardContent>
                <CardActions>
                    <Button onClick={() => props.deleteHandler(props.id)}>
                        Delete
                    </Button>
                    <Button onClick={() => props.moveHandler(props.id, -1)}>
                        Up
                    </Button>
                    <Button onClick={() => props.moveHandler(props.id, 1)}>
                        Down
                    </Button>
                    <Button onClick={() => props.editHandler(props.id)}>
                        Edit
                    </Button>
                </CardActions>
            </div>
        );
    }

    return (
        <Card className="todo-item">
            { props.loading ?
                <CircularProgress className="loading-bar"/>
                : renderBody()
            }
        </Card>
    );
}