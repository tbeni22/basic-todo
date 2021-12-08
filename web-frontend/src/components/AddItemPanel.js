import React from "react";
import {Button, Card, Grid, TextField,} from "@mui/material";
import AddIcon from '@mui/icons-material/Add';

export class AddItemPanel extends React.Component {
    constructor(props) {
        super(props);
        this.saveHandler = props.saveHandler;
        this.state = {
            title: "",
            itemState: "pending",
        };
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
                                onClick={() => this.saveHandler(this.state)}>
                            Add
                        </Button>
                    </Grid>
                </Grid>
            </Card>
        );
    }
}