import React from "react";
import {Button, Card, Grid, TextField, ToggleButton, ToggleButtonGroup} from "@mui/material";
import {LocalizationProvider, DatePicker} from "@mui/lab";
import AdapterDayjs from "@mui/lab/AdapterDayjs";
import SaveOutlinedIcon from '@mui/icons-material/SaveOutlined';

export class EditView extends React.Component {
    constructor(props) {
        super(props);
        this.saveHandler = props.saveHandler;
        this.state = {
            title: "",
            itemState: null,
            date: null,
            desc: "",
        };
    }

    // item state selector handler
    handleStateChange(event, newVal) {
        if (newVal != null)
            this.setState({ itemState: newVal });
    }

    renderStateToggle() {
        return (
            <ToggleButtonGroup
                color="primary"
                exclusive
                size="small"
                value={this.state.itemState}
                onChange={(event, newVal) => this.handleStateChange(event, newVal)}
            >
                <ToggleButton value="pending">Pending</ToggleButton>
                <ToggleButton value="progress">In progress</ToggleButton>
                <ToggleButton value="done">Done</ToggleButton>
                <ToggleButton value="postponed">Postponed</ToggleButton>
            </ToggleButtonGroup>
        );
    }

    renderDateSelector() {
        return (
            <LocalizationProvider dateAdapter={AdapterDayjs}>
                <DatePicker
                    label="Deadline"
                    value={this.state.date}
                    onChange={(newVal) => {this.setState({ date: newVal })}}
                    renderInput={(props) => <TextField {...props} />}
                />
            </LocalizationProvider>
        );
    }

    render() {
        return(
            <Card id="edit-panel" className="todo-item">
                <Grid container
                      spacing={2}
                      justifyContent="center"
                      alignItems="center"
                >
                    <Grid item container
                          xs={6}
                          spacing={2}
                          direction="column"
                          alignItems="center"
                    >
                        <Grid item>
                            <TextField id="title-field"
                                       label="Title"
                                       value={this.state.title}
                                       onChange={(e, newVal) =>
                                            {this.setState({ title: newVal })}
                                       }
                            />
                        </Grid>
                        <Grid item>
                            {this.renderDateSelector()}
                        </Grid>
                    </Grid>

                    {/* todo: az onchange és value párosok miatt overlappel a label és a tartalom a textboxokban (title és desc) */}
                    <Grid item>
                        <TextField
                            label="Description"
                            multiline
                            rows={4}
                            value={this.state.desc}
                            onChange={(e, newVal) =>
                                {this.setState({ desc: newVal })}
                            }
                        />
                    </Grid>

                    <Grid item>
                        {this.renderStateToggle()}
                    </Grid>
                    <Grid item>
                        <Button variant="contained"
                                startIcon={<SaveOutlinedIcon/>}
                                onClick={() => this.saveHandler()}>
                            Save
                        </Button>
                    </Grid>
                </Grid>
            </Card>
        );
    }
}