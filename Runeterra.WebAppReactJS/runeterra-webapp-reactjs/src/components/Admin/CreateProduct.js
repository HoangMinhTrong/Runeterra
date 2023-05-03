import React, {useState} from 'react';
import axios from 'axios';
import {v4} from "uuid";
import 'bootstrap/dist/css/bootstrap.min.css';
import Button from 'react-bootstrap/Button';
import Form from 'react-bootstrap/Form';

const baseURL = "https://localhost:7241/gateway/product";
export default function CreateProduct(){
    const [id, setId] = useState(v4)
    const [name, setName] = useState("")
    const [description, setDescription] = useState("")
    const [price, setPrice] = useState("")
    const [status, setStatus] = useState("")
    const [quantity, setQuantity] = useState("")
    const [imageId, setImageId] = useState("")
    const [productTypeId, setProductTypeId] = useState("")
    const [storeId, setStoreId] = useState("")


    const addProduct = async (e) => {
        e.preventDefault();
        try {
            const resp = await axios.post(baseURL,{
                id: id,
                name: name,
                description: description,
                price: price,
                status: status,
                quantity: quantity,
                imageId: imageId,
                productTypeId: productTypeId,
                storeId: storeId})
            console.log(resp.data)
            setName('')
            setDescription('')
            setPrice('')
            setStatus('')
            setQuantity('')
            setImageId('')
            setProductTypeId('')
            setStoreId('')
        } catch (error){
            console.log(error.response)
        }
    }
  return(
      <section className="py-3">
          <h2 className='text-center'>Add Product</h2>
                  <div className="container px-4 px-lg-5 mt-5">
                      <div className="row gx-4 gx-lg-5 row-cols-2 row-cols-md-3 row-cols-xl-4 justify-content-center">
                              <Form  className='form w-50' onSubmit={addProduct}>
                                  <Form.Group className='form-row'>
                                      <Form.Label htmlFor='name' className='form-label'>
                                          Name
                                      </Form.Label>
                                      <Form.Control
                                          type='text'
                                          className='form-input'
                                          id='name'
                                          value={name}
                                          onChange={(e) => setName(e.target.value)}
                                      />
                                  </Form.Group>
                                  <Form.Group className='form-row'>
                                      <Form.Label htmlFor='description' className='form-label'>
                                          Description
                                      </Form.Label>
                                      <Form.Control
                                          type='text'
                                          className='form-input'
                                          id='description'
                                          value={description}
                                          onChange={(e) => setDescription(e.target.value)}
                                      />
                                  </Form.Group>
                                  <Form.Group className='form-row'>
                                      <Form.Label htmlFor='price' className='form-label'>
                                          Price
                                      </Form.Label>
                                      <Form.Control
                                          type='number'
                                          className='form-input'
                                          id='price'
                                          value={price}
                                          onChange={(e) => setPrice(e.target.value)}
                                      />
                                  </Form.Group>

                                  <Form.Group className='form-row'>
                                      <Form.Label htmlFor='status' className='form-label'>
                                          Status
                                      </Form.Label>
                                      <Form.Control
                                          type='text'
                                          className='form-input'
                                          id='status'
                                          value={status}
                                          onChange={(e) => setStatus(e.target.value)}
                                      />
                                  </Form.Group>
                                  <Form.Group className='form-row'>
                                      <Form.Label htmlFor='quantity' className='form-label'>
                                          Quantity
                                      </Form.Label>
                                      <Form.Control
                                          type='number'
                                          className='form-input'
                                          id='status'
                                          value={quantity}
                                          onChange={(e) => setQuantity(e.target.value)}
                                      />
                                  </Form.Group>
                                  <Form.Group className='form-row'>
                                      <Form.Label htmlFor='image' className='form-label'>
                                          Image
                                      </Form.Label>
                                      <Form.Control
                                          type='text'
                                          className='form-input'
                                          id='image'
                                          value={imageId}
                                          onChange={(e) => setImageId(e.target.value)}
                                      />
                                  </Form.Group>
                                  <Form.Group className='form-row'>
                                      <Form.Label htmlFor='productType' className='form-label'>
                                          Product Type
                                      </Form.Label>
                                      <Form.Control
                                          type='text'
                                          className='form-input'
                                          id='image'
                                          value={productTypeId}
                                          onChange={(e) => setProductTypeId(e.target.value)}
                                      />
                                  </Form.Group>
                                  <Form.Group className='form-row'>
                                      <Form.Label htmlFor='store' className='form-label'>
                                          Store
                                      </Form.Label>
                                      <Form.Control
                                          type='text'
                                          className='form-input'
                                          id='store'
                                          value={storeId}
                                          onChange={(e) => setStoreId(e.target.value)}
                                      />
                                  </Form.Group>
                                  <div className="row gx-4 gx-lg-5 row-cols-2 row-cols-md-3 row-cols-xl-4 justify-content-center">
                                      <Button variant="primary" type='submit' className='btn btn-block mt-5'>
                                          Add Product
                                      </Button>
                                  </div>

                              </Form>
                          </div>

                  </div>
      </section>
  )
}