import './App.css';
import {ListView} from "./components/ListView";
import {AddItemPanel} from "./components/AddItemPanel";
import {EditDialog} from "./components/EditDialog";
import {Divider} from "@mui/material";
import React from "react";

export default class App extends React.Component {
    constructor(props) {
        super(props);
        this.state = {
            loading: true,
            items: [],
            editDialogOpen: false
        }
        this.selectedItem = null
    }

    // converts the locally used item object to the appropriate form for the REST API
    convertToDto(item) {
        return {
            id: item.id,
            title: item.title,
            description: item.desc,
            deadline: item.date != null ? new Date (item.date).toISOString() : null,
            categoryName: item.itemState
        }
    }

    // add item to backend
    async addItem(newItem) {
        const response = await fetch("http://localhost:5000/todos/", {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
            },
            body: JSON.stringify(newItem)
        })
        const data = await response.json()
        if (response.ok)
            return data.id;
        else {
            throw Error("Post failed")
        }
    }

    addItemHandler(newItem) {
        const newTodo = this.convertToDto(newItem)
        this.addItem(newTodo).then((id) => {
            newTodo.id = id
            this.setState({ items: this.state.items.concat([newTodo])})
        })
    }

    // update item on backend
    async updateItem(item) {
        const response = await fetch("http://localhost:5000/todos/" + item.id, {
            method: 'PUT',
            headers: {
                'Content-Type': 'application/json',
            },
            body: JSON.stringify(item)
        })
        if (!response.ok)
            throw Error("Post failed")
    }

    moveItemHandler(idx, dir) {
        let itemsCopy = this.state.items
        let newIdx = idx + dir

        itemsCopy[idx].orderNumber += dir
        itemsCopy[newIdx].orderNumber -= dir

        // update in db
        this.updateItem(itemsCopy[idx]).then()
        this.updateItem(itemsCopy[newIdx]).then()

        // update locally
        let tmp = itemsCopy[idx]
        itemsCopy[idx] = itemsCopy[newIdx]
        itemsCopy[newIdx] = tmp

        this.setState({
            items: itemsCopy
        })
    }

    updateItemHandler(item) {
        this.closeEditDialog()
        let data = this.convertToDto(item)
        this.updateItem(data)
            .then(() => {
                let idx = this.state.items.findIndex(i => i.id === data.id)
                const arr = this.state.items
                arr.splice(idx, 1, data)
                this.setState({items: arr})
            })
    }

    closeEditDialog() {
        this.setState({
            editDialogOpen: false
        })
    }

    openEditDialog(id) {
        this.selectedItem = this.state.items.find(item => item.id === id)
        this.setState({
            editDialogOpen: true
        })
    }

    // delete item from backend
    async deleteItem(id) {
        const response = await fetch("http://localhost:5000/todos/" + id, {
            method: 'DELETE'
        })
        return response.status
    }

    removeItemHandler(id) {
        this.deleteItem(id)
            .then(() => {
                let itemsCopy = this.state.items
                let idx = itemsCopy.findIndex(item => item.id === id)
                itemsCopy.splice(idx, 1)
                this.setState({
                    items: itemsCopy
                })
            })
    }

    // fetch item list from backend
    async getList() {
        const response = await fetch("http://localhost:5000/todos/");
        const data = await response.json();
        this.setState({ items: data });
        return response.status === 200
    }

    componentDidMount() {
        this.getList().then((success) => {
            if (success) {
                this.setState({loading: false})
                console.log("list loaded")
            }
        }).catch(() =>
            this.setState({loading: null})
        )
    }

    render() {
        return (
         <div className="App">
            <div id="top-panel">
                <h1>Tasks to do</h1>
                <AddItemPanel saveHandler={this.addItemHandler.bind(this)}/>
                <Divider variant="middle"/>
            </div>
            <ListView items={this.state.items}
                      loading={this.state.loading}
                      editHandler={data => this.openEditDialog(data)}
                      removeHandler={id => this.removeItemHandler(id)}
                      moveHandler={(id, dir) => this.moveItemHandler(id, dir)}
            />
            <EditDialog isOpen={this.state.editDialogOpen}
                        saveHandler={this.updateItemHandler.bind(this)}
                        handleClose={this.closeEditDialog.bind(this)}
                        item={this.selectedItem}
            />
        </div> );
    }
}