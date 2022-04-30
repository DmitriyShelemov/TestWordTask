import { FC } from "react"
import { Form, Button } from 'react-bootstrap';
import { useActions } from "../../hooks/useActions";
import styles from './TranslatedContent.module.scss'

const TranslatedContent: FC<{ translated: string[] | undefined }> = ({ translated }) => {

    const {setTexts} = useActions()

    return (
        <Form className={styles.details}>  
            {translated?.map((text, idx) => (
            <Form.Group className="mb-3" key={`fragment_${idx}`}>
                <Form.Text className={styles.fragment}>{text}</Form.Text>
            </Form.Group>
            ))}
            <Button variant="secondary" size="lg" active onClick={() => setTexts(undefined)}>Close</Button>
        </Form>
    );
  }
  
  export default TranslatedContent;