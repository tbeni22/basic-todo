import * as React from 'react';
import {EditView} from "./EditView";
import {Dialog, DialogTitle, DialogActions} from "@mui/material";

export class EditDialog extends React.Component {
    render() {
        if (this.props.item == null)
            return null
        let item = this.props.item

        return (
            <Dialog open={this.props.isOpen} onClose={this.props.handleClose}>
                <DialogTitle>Editing item - {item.title}</DialogTitle>
                <EditView itemData={item}
                          saveHandler={this.props.saveHandler}
                          cancelHandler={this.props.handleClose}
                />
                <DialogActions>{null}</DialogActions>
            </Dialog>
        )
    }
}