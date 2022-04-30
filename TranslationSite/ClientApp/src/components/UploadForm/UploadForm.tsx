import { useState, useEffect } from "react"
import { Form, Button, Toast } from 'react-bootstrap';
import { useActions } from "../../hooks/useActions";
import { useTranslateMutation, useGetLanguagesQuery } from "../../store/translations/translation.api";
import styles from './UploadForm.module.scss'

const UploadForm = () => {

    const [showToast, setShowToast] = useState<boolean>(false)
    const toggleToast = () => setShowToast(!showToast)
    const [err, setErr] = useState<any>()
    const { data, isLoading } = useGetLanguagesQuery(90)

    const [urlSelected, setUrlSelected] = useState<boolean>(false)
    const [from, setFrom] = useState<string>('en')
    const [to, setTo] = useState<string>('ru')
    const [file, setFile] = useState<any>()
    const [fileName, setFileName] = useState<any>()
    const [fileUrl, setFileUrl] = useState<string>()

    const saveFile = (e: any) => {
        if (e.target.files.length < 1) {
            return
        }

        var fileData = e.target.files[0]
        setFile(fileData)
        setFileName(fileData.name)
    }
   
    const {setTexts} = useActions()
    const [translate] = useTranslateMutation()
    const submit = (e: any) => {
        e.preventDefault()

        if (from && to && ((file && fileName) || fileUrl)) {

            if (!urlSelected && file && file.size/(1024*1024) > 3) {
            
                setErr({ data: 'File size exceed 3Mb. Please choose different file'})
                setShowToast(true)
                return
            }

            if (!urlSelected && file && file.size/(1024*1024) === 0) {
            
                setErr({ data: 'File size is 0Mb. Please choose different file'})
                setShowToast(true)
                return
            }

            const request = new FormData();
            request.append("From", from);
            request.append("To", to);

            if (urlSelected && fileUrl) {
                request.append("FileUrl", fileUrl);
            } else {
                request.append("FileName", fileName)
                request.append("FormFile", file);
            }

            translate(request)
            .unwrap()
            .then((payload) => {
                setTexts(payload)
            })
            .catch((error) => {
                if ((error?.data as string).startsWith('<!DOCTYPE')) {
                    setErr({ data: 'Unexpected error occurred'})
                } else {
                    setErr(error as any)
                }

                setShowToast(true)
            })
        } else {
            
            setErr({ data: 'Please choose document'})
            setShowToast(true)
        }
    }

    return (
        <div>
            {isLoading
            ? (
                'Loading...'
            ) : (
                <div>
                    <Toast className={styles.alert} onClose={toggleToast} show={showToast} animation={false}>
                        <Toast.Header>
                            <strong className="me-auto">Alert</strong>
                        </Toast.Header>
                        <Toast.Body>{err?.data}</Toast.Body>
                    </Toast>
                    <Form className={styles.upload} onSubmit={submit}>
                        <Form.Group className="mb-3" controlId="from">
                            <Form.Label>Language from:</Form.Label>                
                            <Form.Select required={true} onChange={e => setFrom(e.target.value)}>
                                {data?.map((lang, idx) => (
                                <option key={`from_${idx}`} selected={lang.code === from} value={lang.code}>{lang.code} '|' {lang.name}</option>
                                ))}
                            </Form.Select>
                        </Form.Group>
                        <Form.Group className="mb-3" controlId="to">
                            <Form.Label>Language to:</Form.Label>                
                            <Form.Select required={true} onChange={e => setTo(e.target.value)}>
                                {data?.map((lang, idx) => (
                                <option key={`to_${idx}`} selected={lang.code === to} value={lang.code}>{lang.code} '|' {lang.name}</option>
                                ))}
                            </Form.Select>
                        </Form.Group>
                        <Form.Group className="mb-3" controlId="formFile">
                            <Form.Label>Specify Word document</Form.Label>
                            <Form.Check type="radio" label="Upload file:" checked={!urlSelected} onChange={() => setUrlSelected(false)}/>
                            <Form.Control type="file" disabled={urlSelected} required={!urlSelected} placeholder="Choose document" accept=".doc,.docx" onChange={saveFile} />
                        </Form.Group>
                        <Form.Group className="mb-3" controlId="formUrl">
                            <Form.Check type="radio" label="URL:" checked={urlSelected} onChange={() => setUrlSelected(true)}/>
                            <Form.Control type="url" disabled={!urlSelected} required={urlSelected} placeholder="http://" onChange={e => setFileUrl(e.target.value)} />
                        </Form.Group>
                        <Button variant="primary" type="submit">Translate</Button>
                    </Form>
                </div>)}
        </div>
    );
  }
  
  export default UploadForm;