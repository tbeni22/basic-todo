import * as React from 'react';
import Button from '@mui/material/Button';
import Dialog from '@mui/material/Dialog';
import DialogActions from '@mui/material/DialogActions';
import DialogTitle from '@mui/material/DialogTitle';
import {EditView} from "./EditView";
//import SaveOutlinedIcon from "@mui/icons-material/SaveOutlined";

export class EditDialog extends React.Component {
    render() {
        if (this.props.item == null)
            return null
        let item = this.props.item
        return (
            <Dialog open={this.props.isOpen} onClose={this.props.handleClose}>
                <DialogTitle>Editing item - {item.title}</DialogTitle>
                <EditView itemData={item} saveHandler={this.props.saveHandler}/>
                <DialogActions>
                    <Button onClick={this.props.handleClose}>Cancel</Button>
                    {/*<Button variant="contained"
                            startIcon={<SaveOutlinedIcon/>}
                            onClick={_ => this.props.saveHandler(item)}>
                        Save
                    </Button>*/}
                </DialogActions>
            </Dialog>
        )
    }
}