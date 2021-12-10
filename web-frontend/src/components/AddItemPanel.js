import React from "react";
import {Button, Card, Grid, TextField,} from "@mui/material";
import AddIcon from '@mui/icons-material/Add';
import PropTypes from "prop-types";

export class AddItemPanel extends React.Component {
    static propTypes = {
        saveHandler: PropTypes.func
    };

    constructor(props) {
        super(props);
        this.saveHandler = props.saveHandler;
        this.state = {
            title: "",
            itemState: "pending",
        };
    }

    initiateSave() {
        this.setState({
            title: ""
        })  // clear title input field
        this.saveHandler(this.state)
    }

    render() {
        return(
            <Card id="add-panel" variant="outlined">
                <Grid container
                      spacing={2}
                      justifyContent="center"
                      alignItems="center"
                >
                    <Grid item>
                        Add a new item:
                    </Grid>
                    <Grid item>
                        <TextField id="title-field"
                                   label="Title"
                                   value={this.state.title}
                                   onChange={(event) =>
                                        {this.setState({ title: event.target.value })}
                                   }
                        />
                    </Grid>
                    <Grid item>
                        <Button variant="contained"
                                startIcon={<AddIcon/>}
                                onClick={this.initiateSave.bind(this)}>
                            Add
                        </Button>
                    </Grid>
                </Grid>
            </Card>
        );
    }
}