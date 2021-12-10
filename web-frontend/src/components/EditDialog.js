import * as React from 'react';
import {EditView} from "./EditView";
import {Dialog, DialogTitle, DialogActions} from "@mui/material";
import PropTypes from "prop-types";

export class EditDialog extends React.Component {
    static propTypes = {
        saveHandler: PropTypes.func,
        handleClose: PropTypes.func,
        item: PropTypes.object,
        isOpen: PropTypes.bool
    };

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